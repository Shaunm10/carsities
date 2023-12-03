import React from 'react';

type Props = { auction: any };
export const AuctionCard = (props: Props) => {
	return (
		<div>
			<h3>{props.auction.make}</h3>
		</div>
	);
};
