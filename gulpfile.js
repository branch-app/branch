const gulp = require('gulp');
const sass = require('gulp-sass');

const paths = {
	sass: ['assets/sass/**/*.scss'],
};

gulp.task('sass', () => {
	gulp.src(paths.sass)
		.pipe(sass().on('error', sass.logError))
		.pipe(gulp.dest('./public/css/'));
});

gulp.task('watch', () => {
	gulp.watch(paths.sass, ['sass']);
});

gulp.task('default', ['sass']);
