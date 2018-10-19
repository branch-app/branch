import memoize from 'memoize-one';
import * as React from 'react';

interface IState {
	animator: string;
	opacity: number;
	renderedChildren: JSX.Element | JSX.Element[];
	timeoutHandle: number | null;
}

export interface IProps {
	children: JSX.Element | JSX.Element[];
	className: string;
	duration: number;
	animator: string;
	tag: string;
}

export default class Fader extends React.PureComponent<IProps, IState> {
	static defaultProps = {
		className: '',
		tag: 'div',
	};

	static getDerivedStateFromProps(props: IProps, state: IState) {
		if (props.animator === state.animator)
			return null;

		return {
			...state,
			opacity: 0,
		};
	}

	constructor(props: IProps) {
		super(props);

		this.state = {
			animator: props.animator,
			opacity: 1,
			renderedChildren: props.children,
			timeoutHandle: null,
		};
	}

	public componentWillUnmount() {
		const { timeoutHandle } = this.state;

		if (!timeoutHandle)
			return;

		window.clearTimeout(timeoutHandle);
	}

	public componentDidUpdate(_: IProps, prevState: IState) {
		if (prevState.opacity === 1 && this.state.opacity === 0) {
			const { children, duration, animator } = this.props;

			console.log(children);

			this.setState({
				animator,
				timeoutHandle: window.setTimeout(() => this.setNewValue(children), duration / 2),
			});
		}
	}

	public render() {
		const {
			className,
			duration,
			tag: Tag,
		} = this.props;
		const {
			opacity,
			renderedChildren,
		} = this.state;
		const transitionDuration = this.getTransDuration(duration);
		const style: {} = {
			opacity,
			transition: `opacity ${transitionDuration}`,
		};

		return (
			<Tag
				className={className}
				style={style}
			>
				{renderedChildren}
			</Tag>
		);
	}

	private setNewValue = (value: JSX.Element | JSX.Element[]) => this.setState({ renderedChildren: value, opacity: 1 });

	private getTransDuration = memoize(duration => `${((duration / 2) / 1000).toFixed(2)}s`);
}
