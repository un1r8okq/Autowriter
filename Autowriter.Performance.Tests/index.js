import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 10,       // Simulate 100 users
    duration: '1m', // Run for one minute
    thresholds: {
        http_req_failed: ['rate<0.01'],     // Fewer than 1% of requests should fail
        http_req_duration: ['p(99)<200'],   // 99% of requests should be faster than 200ms
    },
};

const baseUrl = 'http://localhost:5067';

export default function () {
    var email = `${getRandomString()}@example.com`;
    var password = getRandomString();
    register(email, password);
    login(email, password);

    var res = postProtectedForm(`${baseUrl}/generate`, { wordcount: 1000 });
    check(res, { 'status was 200': (r) => r.status == 200 });
    check(res, { 'contains generated text': (r) => /<h2>Generated writing<\/h2>/.test(r.body) });
}

function getRandomString() {
    return Math.random().toString(16).substring(2, 16);
}

function register(email, password) {
    const url = `${baseUrl}/User/Register`;
    const res = http.get(url);
    res.submitForm({
        formSelector: 'form',
        fields: {
            Email: email,
            Password: password,
        },
    });
}

function login(email, password) {
    const url = `${baseUrl}/User/Login`;
    const res = http.get(url);
    res.submitForm({
        formSelector: 'form',
        fields: {
            email: email,
            password: password,
        },
    });
}

function postProtectedForm(url, body) {
    const getRes = http.get(`${baseUrl}/generate`);
    const xsrfTokenRegex = /__RequestVerificationToken" type="hidden" value="(.+)"/;
    const xsrfToken = getRes.body.match(xsrfTokenRegex)[1];

    var postBody = { __RequestVerificationToken: xsrfToken };
    Object.assign(postBody, body);
    return http.post(url, postBody);
}
