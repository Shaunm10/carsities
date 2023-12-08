import React from 'react';
import { AuctionCard } from './AuctionCard';

async function GetData() {
  const res = await fetch('http://localhost:6001/search?pageSize=10');

  if (!res.ok) {
    throw new Error('Unable to get data');
  }

  const json = await res.json();

  return json;
}

export const Listings = async () => {
  const auctions = await GetData();
  return (
    <div className="grid grid-cols-4 gap-6">
      {auctions &&
        auctions.results.map((auction: any) => (
          <AuctionCard auction={auction} key={auction.Id} />
        ))}
    </div>
  );
};
