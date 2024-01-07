import { EmptyFilter } from '@/app/components/EmptyFilter';
import React from 'react';

const Signin = ({
  searchParams,
}: {
  searchParams: { callbackUrl: string };
}) => {
  return (
    <EmptyFilter
      title='You need to perform this action'
      subtitle='Please click below to sign in'
      showLogin
      callbackUrl={searchParams.callbackUrl}
    />
  );
};

export default Signin;
