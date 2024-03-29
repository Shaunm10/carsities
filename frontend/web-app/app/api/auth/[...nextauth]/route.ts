import NextAuth, { NextAuthOptions } from 'next-auth';
import DuendeIdentityServer6 from 'next-auth/providers/duende-identity-server6';

/**
 * This is configuration and Events for Next.js Authentication.
 */
export const authOptions: NextAuthOptions = {
  session: {
    strategy: 'jwt',
  },
  providers: [
    DuendeIdentityServer6({
      id: 'id-server', // how we identify our provider in our app.
      clientId: 'nextApp', // must match what we called in our IDP's configuration
      clientSecret: 'secret', // must match what we called in our IDP's configuration
      issuer: 'http://localhost:5000', // the url of our IDP
      authorization: {
        params: {
          scope: 'openid profile auctionApp',
        },
      },
      idToken: true, // this will ask the IDP for the userId
    }),
  ],
  callbacks: {
    // This callback is fired called whenever a JSON Web Token is created
    async jwt({ token, profile, account, user }) {
      if (profile) {
        token.username = profile.username;
      }

      // if we have an account + a token
      if (account?.access_token) {
        token.access_token = account.access_token;
      }

      console.log({ token, profile, account, user });
      return token;
    },
    // this callback is fired when a session is checked.
    async session({ session, token }) {
      if (token) {
        session.user.username = token.username;
      }

      return session;
    },
  },
};

const handler = NextAuth(authOptions);

export { handler as GET, handler as POST };
