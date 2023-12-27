import { filterByValues } from '@/types/filterBy';
import { IconType } from 'react-icons';
import { BsStopwatchFill } from 'react-icons/bs';
import { GiFinishLine, GiFlame } from 'react-icons/gi';

export const filterButtons: {
  label: string;
  icon: IconType;
  value: filterByValues;
}[] = [
  {
    label: 'Live Auctions',
    icon: GiFlame,
    value: 'live',
  },
  {
    label: 'Ending < 6 hours',
    icon: GiFinishLine,
    value: 'endingSoon',
  },
  {
    label: 'Completed',
    icon: BsStopwatchFill,
    value: 'finished',
  },
];
