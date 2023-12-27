'use client';
import React, { useEffect, useState } from 'react';
import { AuctionCard } from './AuctionCard';
import { Auction, PagedResults } from '@/types';
import { AppPagination } from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import { Filters } from './Filters';

import { shallow } from 'zustand/shallow';
import qs from 'query-string';
import { useParamsStore } from '@/hooks/useParamsStore';
import { Heading } from '../components/Heading';
import { EmptyFilter } from '../components/EmptyFilter';

export const Listings = () => {
  const [data, setData] = useState<PagedResults<Auction>>();

  const params = useParamsStore(
    (state) => ({
      pageNumber: state.pageNumber,
      pageCount: state.pageCount,
      searchTerm: state.searchTerm,
      pageSize: state.pageSize,
      orderBy: state.orderBy,
      filterBy: state.filterBy,
    }),
    shallow
  );

  const setParams = useParamsStore((state) => state.setParams);
  const url = qs.stringifyUrl({
    url: '',
    query: params,
  });

  const setPageNumber = (pageNumber: number) => {
    setParams({ pageNumber });
  };

  useEffect(() => {
    // call the network service to search for auctions
    getData(url).then((data) => {
      // and set the data.
      setData(data);
    });
  }, [url]);

  if (!data) {
    return <h3>Loading...</h3>;
  }

  return (
    <>
      <Filters />
      {data.totalCount === 0 ? (
        <EmptyFilter showReset />
      ) : (
        <>
          <div className='grid grid-cols-4 gap-6'>
            {data.results &&
              data.results.map((auction) => (
                <AuctionCard auction={auction} key={auction.id} />
              ))}
          </div>
          <div className='flex justify-center mt-4'>
            <AppPagination
              pageChanged={setPageNumber}
              currentPage={params.pageNumber}
              pageCount={data.pageCount}
            />
          </div>
        </>
      )}
    </>
  );
};
