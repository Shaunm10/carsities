'use server';
import { Heading } from '@/app/components/Heading';
import React from 'react';
import { AuctionForm } from '../../AuctionForm';
import { getDetailedViewData } from '@/app/actions/auctionActions';
import { getCurrentUser } from '@/app/actions/authActions';

type props = { params: { id: string } };

const Update = async ({ params }: props) => {
  const auction = await getDetailedViewData(params.id);
  const user = await getCurrentUser();
  const isUserAllowed = auction?.seller === user?.username;
  return (
    <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
      {isUserAllowed && (
        <>
          <Heading
            title='Update your auction'
            subtitle='Please update the details for your car'
          />

          <AuctionForm auction={auction} />
        </>
      )}
    </div>
  );
};

export default Update;
