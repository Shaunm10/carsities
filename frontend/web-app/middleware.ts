export { default } from 'next-auth/middleware';

// terribly documented/named this is to protect routes from unauthenticated access.
export const config = {
  matcher: ['/session'],
  pages: {
    signIn: '/api/auth/signin',
  },
};
