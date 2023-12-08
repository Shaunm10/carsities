import React from 'react';
import { AuctionCard } from './AuctionCard';
import { Auction, PagedResults } from '@/types';
import { AppPageRouteModule } from 'next/dist/server/future/route-modules/app-page/module.compiled';
import { AppPagination } from '../components/AppPagination';

async function GetData(): Promise<PagedResults<Auction>> {
  const res = await fetch('http://localhost:6001/search?pageSize=4');

  if (!res.ok) {
    throw new Error('Unable to get data');
  }

  const json = await res.json();

  return json;
}

export const Listings = async () => {
  const data = await GetData();

  return (
    <>
      <div className="grid grid-cols-4 gap-6">
        {data &&
          data.results.map((auction) => (
            <AuctionCard auction={auction} key={auction.id} />
          ))}
      </div>
      <div className="flex justify-center mt-4">
        <AppPagination currentPage={1} pageCount={data.pageCount} />
      </div>
    </>
  );
};
