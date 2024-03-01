import { getTokenWorkAround } from '@/app/actions/authActions';

const baseUrl = 'http://localhost:6001/';

async function put(url: string, body: {}) {
  const requestOptions: RequestInit = {
    method: 'PUT',
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function del(url: string) {
  const requestOptions: RequestInit = {
    method: 'DELETE',
    headers: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function post(url: string, body: {}) {
  const requestOptions: RequestInit = {
    method: 'POST',
    headers: await getHeaders(),
    body: JSON.stringify(body),
  };

  const response = await fetch(baseUrl + url, requestOptions);
  return await handleResponse(response);
}

async function get(url: string) {
  const requestOptions: RequestInit = {
    method: 'GET',
    headers: await getHeaders(),
  };

  const response = await fetch(baseUrl + url, requestOptions);

  return await handleResponse(response);
}

/**
 * Helper function to get the headers to be passed to the server
 */
async function getHeaders() {
  const token = await getTokenWorkAround();
  const headers = {
    'Content-type': 'application/json',
  } as any;

  if (token) {
    headers.Authorization = `Bearer ${token.access_token}`;
  }

  return headers;
}

// async function delete(url: string){}

async function handleResponse(response: Response) {
  const text = await response.text();
  const data = text && JSON.parse(text);

  if (response.ok) {
    // the response could have data, or just be an "OK"
    return data || response.statusText;
  } else {
    // otherwise just send back and object wrapping the error
    const error = {
      status: response.status,
      message: response.statusText,
    };

    // this will give us the error property that can be checked by
    // the consuming component.
    return { error };
  }
}

// finally expose it to consumers
export const fetchWrapper = {
  get,
  post,
  put,
  del,
};
