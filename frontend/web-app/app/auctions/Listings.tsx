import React from 'react';

async function GetData() {
	const res = await fetch('http://localhost:6001/search');

	if (!res.ok) {
		throw new Error('Unable to get data');
	}

	return res.json();
}

const Listings = async () => {
	const data = await GetData();
	return <div>{JSON.stringify(data, null, 2)}</div>;
};
export default Listings;
