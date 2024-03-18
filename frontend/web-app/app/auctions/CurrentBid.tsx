import React from 'react';

type Props = {
  amount?: number;
  reservePrice: number;
};

const CurrentBid = ({ amount, reservePrice }: Props) => {
  const text = amount ? '$' + amount : 'No bids';
  const color = amount
    ? amount > reservePrice
      ? 'bg-green-600'
      : 'bg-amber-600'
    : 'bg-red-600';

  return <div>CurrentBid</div>;
};

export default CurrentBid;
