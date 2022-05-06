import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 100,       // Simulate 100 users
    duration: '1m', // Run for one minute
    thresholds: {
        http_req_failed: ['rate<0.001'],     // Fewer than 0.1% of requests should fail
        http_req_duration: ['p(95)<100'],   // 99% of requests should be faster than 200ms
    },
};

const baseUrl = 'http://localhost:5067';

export function setup() {
    const creds = {
        email: `${nanoId()}@example.com`,
        password: nanoId(),
    };

    post(`${baseUrl}/user/register`, creds);

    return creds;
}

export default function(creds) {
    post(`${baseUrl}/user/login`, creds);

    var res = post(`${baseUrl}/generate`, { wordcount: 1000 });
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

function post(url, body) {
    const res = http.get(url);
    const tokenExpr = /__RequestVerificationToken" type="hidden" value="(.+)"/;
    const token = res.body.match(tokenExpr)[1];

    var postBody = { __RequestVerificationToken: token };
    Object.assign(postBody, body);
    return http.post(url, postBody);
}
