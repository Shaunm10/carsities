'use server';
import { getDetailedViewData } from '@/app/actions/auctionActions';
import { Heading } from '@/app/components/Heading';
import React from 'react';

type props = { params: { id: string } };

// NOTE: you should only use async on the component if it's
// a server component.
const Details = async ({ params }: props) => {
  const data = await getDetailedViewData(params.id);

  return (
    <div>
      <Heading title={`${data.make} ${data.model}`} subtitle='' />
    </div>
  );
};

export default Details;
