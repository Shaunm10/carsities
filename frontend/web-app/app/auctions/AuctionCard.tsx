import React from 'react';
import Image from 'next/image';

type Props = { auction: any };
export const AuctionCard = ({ auction }: Props) => {
	return (
		<a href="#">
			<div className="w-full bg-gray-200 aspect-w-16 aspect-h-10 rounded-lg overflow-hidden ">
				<Image
					src={auction.imageUrl}
					alt="image"
					fill
					className="object-cover"
				></Image>
				<h3>{auction.make}</h3>
			</div>
		</a>
	);
};
