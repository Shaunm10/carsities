import React from 'react';
import { AuctionCard } from './AuctionCard';

async function GetData() {
	const res = await fetch('http://localhost:6001/search');

	if (!res.ok) {
		throw new Error('Unable to get data');
	}

	const json = res.json();

	return json;
}

export const Listings = async () => {
	const data = await GetData();
	return (
		<div className="grid grid-cols-4 gap-6">
			{data &&
				data.results.map((auction: any) => (
					<AuctionCard auction={auction} key={auction.Id} />
				))}
		</div>
	);
};
