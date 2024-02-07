import React from 'react';

type props = { params: { id: string } };

const Update = ({ params }: props) => {
  return <div>Update - {params.id} </div>;
};

export default Update;
