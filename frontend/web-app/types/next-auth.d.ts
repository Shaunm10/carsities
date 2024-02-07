import { DefaultSession } from 'next-auth';

// these are Typescript definition files that is allow us to add properties to existing next-auth type definitions

declare module 'next-auth' {
  interface Session {
    user: {
      username: string;
    } & DefaultSession['user'];
  }

  interface Profile {
    username: string;
  }

  interface User {
    username: string;

  }
}

declare module 'next-auth/jwt' {
  interface JWT {
    username: string;
    access_token: string
  }
}
