'use client';
import React, { useState } from 'react';
import { updateAuctionTest } from '../actions/auctionActions';
import { Button } from 'flowbite-react';

export const AuthTest = () => {
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<{} | undefined>({});

  const doUpdate = () => {
    setResult(undefined);
    setLoading(true);
    updateAuctionTest()
      .then((res) => setResult(res))
      //.catch((err) => setResult(undefined))
      .finally(() => setLoading(false));
  };
  return (
    <div className='flex items-center gap-4'>
      <Button outline isProcessing={loading} onClick={doUpdate}>
        Test Auth
      </Button>
      <div>{JSON.stringify(result, null, 2)}</div>
    </div>
  );
};
