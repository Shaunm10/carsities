import React from 'react';
import Image from 'next/image';

type Props = { auction: any };
export const AuctionCard = ({ auction }: Props) => {
	return (
		<a href="#">
			<div className="w-full bg-gray-200 aspect-video rounded-lg overflow-hidden ">
				<Image src={auction.imageUrl} alt="image"></Image>
				<h3>{auction.make}</h3>
			</div>
		</a>
	);
};
