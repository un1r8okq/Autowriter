import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 100,       // Simulate 100 users
    duration: '1m', // Run for one minute
    thresholds: {
        http_req_failed: ['rate<0.001'],     // Fewer than 0.1% of requests should fail
        http_req_duration: ['p(95)<200'],   // 99% of requests should be faster than 200ms
    },
};

const baseUrl = 'http://localhost:5067';

export function setup() {
    const creds = {
        Email: `${nanoId()}@example.com`,
        Password: nanoId(),
    };

    post(`${baseUrl}/user/register`, creds);
    post(`${baseUrl}/user/login`, creds);

    const cookies = http.cookieJar().cookiesForURL(baseUrl);
    const sessionCookie = cookies['.AspNetCore.Identity.Application'][0];
    return { '.AspNetCore.Identity.Application': sessionCookie };
}

export default function(cookies) {
    var res = post(`${baseUrl}/generate`, { wordcount: 1000 }, cookies);
    check(res, { 'status was 200': (r) => r.status == 200 });
    check(res, { 'contains generated text': (r) => /<h2>Generated writing<\/h2>/.test(r.body) });
}

// Insecure ID generation from https://github.com/ai/nanoid/blob/main/non-secure/index.js
// Licensed under MIT
function nanoId() {
    const urlAlphabet = 'useandom-26T198340PX75pxJACKVERYMINDBUSHWOLF_GQZbfghjklqvwyzrict';
    const size = 16;
    let id = '';
    let i = size;
    while (i--) {
        id += urlAlphabet[(Math.random() * 64) | 0];
    }
    return id
}

function post(url, body, cookies = null) {
    const res = http.get(url, { cookies });
    const tokenExpr = /__RequestVerificationToken" type="hidden" value="(.+)"/;
    const token = res.body.match(tokenExpr)[1];

    const postBody = { __RequestVerificationToken: token };
    Object.assign(postBody, body);
    return http.post(url, postBody, { cookies });
}
