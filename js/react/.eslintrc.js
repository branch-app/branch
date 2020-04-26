module.exports = {
	extends: ['../.eslintrc'],

	plugins: [
		'babel',
		'react',
		'@typescript-eslint'
	],

	env: {
		node: false,
		browser: true
	},

	globals: {
		'Promise': false,
		'Set': false,
		'window': false
	},

	parser: '@typescript-eslint/parser',
	parserOptions: {
		ecmaVersion: 2018,
		sourceType: 'module',
		project: './tsconfig.json',
		tsconfigRootDir: __dirname,

		ecmaFeatures: {
			impliedStrict: true,
			jsx: true
		}
	},

	rules: {
		'babel/no-invalid-this': 1,
		'camelcase': 'off',
		'func-style': [1, 'declaration', { 'allowArrowFunctions': true }],
		'indent': 'off',
		'no-array-constructor': 'off',
		'no-confusing-arrow': 0,
		'no-invalid-this': 0,
		'no-magic-numbers': 0,
		'no-unused-vars': 'off',
		'react/jsx-uses-react': 2,
		'react/jsx-uses-vars': 2,
		'react/no-multi-comp': 0,
		'react/prop-types': 0,
		'sort-imports': [1, { 'memberSyntaxSortOrder': ['single', 'all', 'multiple', 'none'] }],

		'@typescript-eslint/adjacent-overload-signatures': 'error',
		'@typescript-eslint/array-type': 'error',
		'@typescript-eslint/ban-types': 'error',
		'@typescript-eslint/camelcase': 'error',
		'@typescript-eslint/class-name-casing': 'error',
		'@typescript-eslint/indent': [1, 'tab', { 'SwitchCase': 1 }],
		'@typescript-eslint/interface-name-prefix': [1, 'never'],
		'@typescript-eslint/member-delimiter-style': 'error',
		'@typescript-eslint/member-naming': 'off',
		'@typescript-eslint/no-angle-bracket-type-assertion': 'error',
		'@typescript-eslint/no-array-constructor': 'error',
		'@typescript-eslint/no-explicit-any': 'warn',
		'@typescript-eslint/no-inferrable-types': 'error',
		'@typescript-eslint/no-misused-new': 'error',
		'@typescript-eslint/no-namespace': 'error',
		'@typescript-eslint/no-object-literal-type-assertion': 'error',
		'@typescript-eslint/no-parameter-properties': 'error',
		'@typescript-eslint/no-triple-slash-reference': 'error',
		'@typescript-eslint/no-unused-vars': 'warn',
		'@typescript-eslint/no-use-before-define': [1, { 'functions': false }],
		'@typescript-eslint/no-var-requires': 'error',
		'@typescript-eslint/prefer-interface': 'warn',
		'@typescript-eslint/restrict-plus-operands': 'error',
		'@typescript-eslint/type-annotation-spacing': 'error'
	},
};
