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

export function setup() {
    const creds = {
        email: `${randomString()}@example.com`,
        password: randomString(),
    };

    post(`${baseUrl}/user/register`, creds);

    return creds;
}

export default function(creds) {
    post(`${baseUrl}/User/Login`, creds);

    var res = post(`${baseUrl}/generate`, { wordcount: 1000 });
    check(res, { 'status was 200': (r) => r.status == 200 });
    check(res, { 'contains generated text': (r) => /<h2>Generated writing<\/h2>/.test(r.body) });
}

function randomString() {
    return Math.random().toString(16).substring(2, 16);
}

function post(url, body) {
    const res = http.get(`${baseUrl}/generate`);
    const tokenExpr = /__RequestVerificationToken" type="hidden" value="(.+)"/;
    const token = res.body.match(tokenExpr)[1];

    var postBody = { __RequestVerificationToken: token };
    Object.assign(postBody, body);
    return http.post(url, postBody);
}
