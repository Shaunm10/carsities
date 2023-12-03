import React from 'react';
import { AuctionCard } from './AuctionCard';

async function GetData() {
	const res = await fetch('http://localhost:6001/search');

	if (!res.ok) {
		throw new Error('Unable to get data');
	}

	return res.json();
}

export const Listings = async () => {
	const data = await GetData();
	return (
		<div>
			{data &&
				data.results.map((auction: any) => (
					<AuctionCard auction={auction} key={auction.Id} />
				))}
		</div>
	);
};
