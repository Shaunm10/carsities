'use server';
import { getDetailedViewData } from '@/app/actions/auctionActions';
import { Heading } from '@/app/components/Heading';
import React from 'react';
import CountdownTimer from '../../CountdownTimer';
import { CarImage } from '../../CarImage';
import DetailedSpecs from './DetailedSpecs';
import EditButton from './EditButton';
import { getCurrentUser } from '@/app/actions/authActions';
import DeleteButton from './DeleteButton';

type props = { params: { id: string } };

// NOTE: you should only use async on the component if it's
// a server component.
const Details = async ({ params }: props) => {
  const auction = await getDetailedViewData(params.id);
  const user = await getCurrentUser();
  const isUserOwnerOfAuction = user?.username === auction.seller;

  return (
    <div>
      {/* Header and countdown */}
      <div className='flex justify-between'>
        <div className='flex items-center gap-3'>
          <Heading title={`${auction.make} ${auction.model}`} />
          {isUserOwnerOfAuction && (
            <>
              <EditButton id={auction.id} />
              <DeleteButton auctionId={auction.id} />
            </>
          )}
        </div>
        <div className='flex gap-3'>
          <h3 className='text-2x1 font-semibold'>Time remaining:</h3>
          <CountdownTimer auctionEnd={auction.auctionEnd} />
        </div>
      </div>

      {/* Image and Bids */}
      <div className='grid grid-cols-2 gap-6 mt-3'>
        <div className='w-full bg-gray-200  aspect-h-10 aspect-w-16 rounded-lg overflow-hidden'>
          <CarImage imageUrl={auction.imageUrl} />
        </div>

        <div className='border-2 rounded-lg p-2 bg-gray-100'>
          <Heading title='Bids' />
        </div>
      </div>

      {/* Detail Specs */}
      <div className='mt-3 grid grid-cols-1 rounded-lg'>
        <DetailedSpecs auction={auction} />
      </div>
    </div>
  );
};

export default Details;
