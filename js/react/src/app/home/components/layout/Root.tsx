import * as React from 'react';
import styled from 'styled-components';

const Wrapper = styled.div`
	margin: 0 auto;
`;

const Root: React.FunctionComponent = ({ children }) => (<Wrapper className={'root'}>{children}</Wrapper>);

export default Root;
