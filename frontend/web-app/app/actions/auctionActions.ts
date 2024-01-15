'use server';
import { Auction, PagedResults } from '@/types';

export async function getData(query: string): Promise<PagedResults<Auction>> {
  const res = await fetch(`http://localhost:6001/search${query}`);

  if (!res.ok) {
    throw new Error('Unable to get data');
  }

  const json = await res.json();

  return json;
}

export async function updateAuctionTest() {

  const data = {
    // a random number from 1 -> 100,000
    milage: Math.floor(Math.random() * 100000) + 1
  }

  const response = await fetch('http://localhost:6001/auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c', {
    method: 'PUT',
    headers: {}, // no way this will work until the header is added.
    body: JSON.stringify(data)
  });

  if (!response.ok) {

    return {
      status: response.status,
      message: response.statusText
    }
  }

  return response.statusText;

}
