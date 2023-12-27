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
