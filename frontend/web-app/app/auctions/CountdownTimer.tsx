'use client';
import React from 'react';
import Countdown from 'react-countdown';

type Props = { auctionEnd: string };
const renderer = ({
  days,
  hours,
  minutes,
  seconds,
  completed,
}: {
  days: number;
  hours: number;
  minutes: number;
  seconds: number;
  completed: boolean;
}) => {
  if (completed) {
    // render a completed state
    return <span>Finished</span>;
  } else {
    // render a countdown
    return (
      <span>
        {days}:{hours}:{minutes}:{seconds}
      </span>
    );
  }
};
const CountdownTimer = ({ auctionEnd }: Props) => {
  return (
    <div>
      <Countdown renderer={renderer} date={auctionEnd} />
    </div>
  );
};

export default CountdownTimer;
