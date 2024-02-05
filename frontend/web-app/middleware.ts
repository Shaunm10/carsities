export { default } from 'next-auth/middleware';

// terribly documented/named config option
// however this is to protect routes from unauthenticated access.
export const config = {
  matcher: ['/session'],
  pages: {
    // this effectively overwrites the page the user is taken to.
    signIn: '/api/auth/signin',
  },
};
