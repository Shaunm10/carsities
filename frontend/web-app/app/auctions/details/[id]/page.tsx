import React from 'react';

type props = { params: { id: string } };

const Details = ({ params }: props) => {
  return <div>Details for {params.id}</div>;
};

export default Details;
