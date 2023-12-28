import { getServerSession } from 'next-auth';
import { authOptions } from '../api/auth/[...nextauth]/route';

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
