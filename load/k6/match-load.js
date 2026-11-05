import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

const BASE_URL = __ENV.BASE_URL || 'http://localhost:8080';
const simulateDuration = new Trend('simulate_duration');

export const options = {
  scenarios: {
    matches: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '10s', target: 10 },
        { duration: '30s', target: 10 },
        { duration: '10s', target: 20 },
        { duration: '30s', target: 20 },
        { duration: '10s', target: 0 }
      ],
      gracefulRampDown: '5s'
    }
  },
  thresholds: {
    http_req_failed: ['rate<0.10'],
    http_req_duration: ['p(95)<4000'],
    'http_req_duration{name:Matches.Index}': ['p(95)<2000'],
    'http_req_duration{name:Matches.Simulate}': ['p(95)<4000'],
    checks: ['rate>0.95'],
    simulate_duration: ['p(95)<4000']
  }
};

export default function () {
  const index = http.get(`${BASE_URL}/Matches`, {
    tags: { name: 'Matches.Index' }
  });

  check(index, {
    'index status 200': (r) => r.status === 200,
    'index contains token': (r) =>
      /name="__RequestVerificationToken"[^>]*value="[^"]+"/.test(r.body)
  });

  const match = index.body.match(
    /name="__RequestVerificationToken"[^>]*value="([^"]+)"/
  );
  if (!match) {
    sleep(1);
    return;
  }

  const response = http.post(
    `${BASE_URL}/Matches/Simulate`,
    { __RequestVerificationToken: match[1] },
    {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      tags: { name: 'Matches.Simulate' }
    }
  );

  simulateDuration.add(response.timings.duration);
  check(response, {
    'simulate status 200': (r) => r.status === 200,
    'simulate returns ok': (r) => Boolean(r.body && r.body.includes('"ok":true'))
  });

  sleep(1);
}
