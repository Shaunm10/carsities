import { useParamsStore } from '@/hooks/useParamsStore';
import { orderByValues } from '@/types/orderBy';
import { Button, ButtonGroup } from 'flowbite-react';

import React from 'react';

import { orderButtons } from './orderButtons';
import { filterButtons } from './filterButtons';

// the number of items per page.
const pageSizeButtons = [4, 8, 12];

export const Filters = () => {
  const pageSize = useParamsStore((state) => state.pageSize);
  const setParams = useParamsStore((state) => state.setParams);
  const orderBy = useParamsStore((state) => state.orderBy);
  const filterBy = useParamsStore((state) => state.filterBy);

  return (
    <div className='flex justify-between items-center mb-4'>
      <div>
        <span className='uppercase text-sm text-gray-300 mr-2'>Filter by</span>
        <Button.Group>
          {filterButtons.map(({ icon: Icon, label, value }) => (
            <Button
              key={value}
              onClick={() => setParams({ filterBy: value })}
              color={`${filterBy === value ? 'red' : 'gray'}`}
            >
              <Icon className='mr-3 h-4 w-4' />
              {label}
            </Button>
          ))}
        </Button.Group>
      </div>

      <div>
        <span className='uppercase text-sm text-gray-300 mr-2'>Order by</span>
        <Button.Group>
          {orderButtons.map(({ icon: Icon, label, value }) => (
            <Button
              key={value}
              onClick={() => setParams({ orderBy: value })}
              color={`${orderBy === value ? 'red' : 'gray'}`}
            >
              <Icon className='mr-3 h-4 w-4' />
              {label}
            </Button>
          ))}
        </Button.Group>
      </div>

      <div>
        <span className='uppercase text-sm text-gray-300 mr-2'>Page Size</span>
        <ButtonGroup>
          {pageSizeButtons.map((value, i) => (
            <Button
              key={i}
              onClick={() => setParams({ pageSize: value })}
              color={`${pageSize === value ? 'red' : 'gray'}`}
              className='focus:ring-0'
            >
              {value}
            </Button>
          ))}
        </ButtonGroup>
      </div>
    </div>
  );
};
