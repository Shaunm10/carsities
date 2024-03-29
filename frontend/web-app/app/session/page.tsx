import React from 'react';
import { getSession, getTokenWorkAround } from '../actions/authActions';
import Head from 'next/head';
import { Heading } from '../components/Heading';
import { AuthTest } from './AuthTest';

/**  This is a next.js convention. When we have folder named 'Session'
 *   which will be the url route "~/session"
 *   NOTE: this has to export as default for this to work.
 */
const Session = async () => {
  const session = await getSession();
  const token = await getTokenWorkAround();
  return (
    <div>
      <Heading title='Session dashboard' subtitle=''></Heading>
      <div className='bg-blue-200 border-2 border-blue-500'>
        <h3 className='text-lg'>Session data</h3>
        <pre>{JSON.stringify(session, null, 2)}</pre>
      </div>
      <div className='mt-4'>
        <AuthTest />
      </div>
      <div className='bg-green-200 border-2 border-blue-500 mt-4'>
        <h3 className='text-lg'>Token data</h3>
        <pre className='overflow-auto'>{JSON.stringify(token, null, 2)}</pre>
      </div>
    </div>
  );
};

export default Session;
