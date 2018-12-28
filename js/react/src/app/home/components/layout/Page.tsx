import * as React from 'react';
import styled from 'styled-components';

const Wrapper = styled.div`
	text-align: center;
`;

const Page: React.FunctionComponent = ({ children }) => (<Wrapper>{children}</Wrapper>);

export default Page;
