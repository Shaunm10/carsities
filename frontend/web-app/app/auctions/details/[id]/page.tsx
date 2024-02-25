'use server';
import { getDetailedViewData } from '@/app/actions/auctionActions';
import { Heading } from '@/app/components/Heading';
import React from 'react';
import CountdownTimer from '../../CountdownTimer';
import { CarImage } from '../../CarImage';
import DetailedSpecs from './DetailedSpecs';

type props = { params: { id: string } };

// NOTE: you should only use async on the component if it's
// a server component.
const Details = async ({ params }: props) => {
  const data = await getDetailedViewData(params.id);

  return (
    <div>
      <div className='flex justify-between'>
        <Heading title={`${data.make} ${data.model}`} />
        <div className='flex gap-3'>
          <h3 className='text-2x1 font-semibold'>Time remaining:</h3>
          <CountdownTimer auctionEnd={data.auctionEnd} />
        </div>
      </div>

      <div className='grid grid-cols-2 gap-6 mt-3'>
        <div className='w-full bg-gray-200  aspect-h-10 aspect-w-16 rounded-lg overflow-hidden'>
          <CarImage imageUrl={data.imageUrl} />
        </div>

        <div className='border-2 rounded-lg p-2 bg-gray-100'>
          <Heading title='Bids' />
        </div>
      </div>
      <div className='mt-3 grid grid-cols-1 rounded-lg'>
        <DetailedSpecs auction={data} />
      </div>
    </div>
  );
};

export default Details;
