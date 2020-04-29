import logo from '../../assets/images/logo/logo-150x150-w.png';
import logoInverse from '../../assets/images/logo/logo-150x150.png';

interface Theme {
	brandPrimary: string;
	brandSecondary: string;
	brandTertiary: string;
	brandQuaternary: string;
	
	brandAccentPrimary: string;
	brandAccentSecondary: string;
	brandAccentTertiary: string;
	
	anchorPrimary: string;
	anchorPrimaryActive: string;
	anchorSecondary: string;
	anchorSecondaryActive: string;
	
	separatorLight: string;

	textInverse: string;

	images: {
		logo: string;
		logoInverse: string;
	};
}

const defaultTheme: Theme = {
	brandPrimary: '#3d3d7c',
	brandSecondary: '#353575',
	brandTertiary: '#5d5da0',
	brandQuaternary: '#aaa4b6',

	brandAccentPrimary: '#e25092',
	brandAccentSecondary: '#b42c69',
	brandAccentTertiary: '#d3508b',

	anchorPrimary: '#3d3d7c',
	anchorPrimaryActive: '#3d3d7c',
	anchorSecondary: '#e25092',
	anchorSecondaryActive: '#d3508b',

	separatorLight: '#eee',

	textInverse: 'white',

	images: {
		logo,
		logoInverse,
	},
};

// yolo this
declare module "styled-components" {
	interface DefaultTheme extends Theme {}
}

export default defaultTheme;
