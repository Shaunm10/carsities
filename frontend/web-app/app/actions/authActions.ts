import { getServerSession } from 'next-auth';
import { authOptions } from '../api/auth/[...nextauth]/route';

import { cookies, headers } from 'next/headers';
import { NextApiRequest } from 'next';
import { getToken } from 'next-auth/jwt';

/**
 * Get's the user's current authentication session
 * @returns Session if available.
 */
export async function getSession() {
  return await getServerSession(authOptions);
}

/**
 * Get's the current user based on the session claims from the IDP
 * @returns The user object or NULL
 */
export async function getCurrentUser() {
  try {
    const session = await getSession();

    if (!session) {
      return null;
    }

    return session.user;
  } catch (error) {
    // if we can't get the session for any reason, than just return null.
    return null;
  }
}

/**
 * This creates a header/cookie combination to create an authenticated
 * Api request.
 */
export async function getTokenWorkAround() {
  const req = {
    // import the headers() and create an object
    headers: Object.fromEntries(headers() as Headers),

    // import the cookies() and create an object
    cookies: Object.fromEntries(
      cookies()
        .getAll()
        .map((c) => [c.name, c.value])
    ),
  } as NextApiRequest;

  return await getToken({ req });
}
