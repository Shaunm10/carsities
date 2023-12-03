import React from 'react';

type Props = { auction: any };
export const AuctionCard = (props: Props) => {
	return (
		<a href="#">
			<div className="w-full bg-gray-200 aspect-video rounded-lg overflow-hidden ">
				<h3>{props.auction.make}</h3>
			</div>
		</a>
	);
};
