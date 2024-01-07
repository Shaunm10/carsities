'use client';
import React, { useState } from 'react';
import { updateAuctionTest } from '../actions/auctionActions';

export const AuthTest = () => {
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<{} | undefined>({});

  const doUpdate = () => {
    setResult(undefined);
    setLoading(true);
    updateAuctionTest()
      .then((res) => setResult(res))
      .catch((err) => setResult(undefined))
      .finally(() => setLoading(false));
  };
  return <div>AuthTest</div>;
};
