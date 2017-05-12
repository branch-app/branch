import babelify from 'babelify';
import browserify from 'browserify';
import buffer from 'vinyl-buffer';
import glob from 'glob';
import gulp from 'gulp';
import merge from 'merge-stream';
import path from 'path';
import sass from 'gulp-sass';
import source from 'vinyl-source-stream';
import sourcemaps from 'gulp-sourcemaps';
import uglify from 'gulp-uglify';

const paths = {
	javascript: {
		src: './assets/javascript/**/*.js',
		dest: './public/javascript/',
		maps: './public/maps/',
	},
	sass: {
		src: ['./assets/sass/main.scss', './assets/sass/**/*.scss'],
		dest: './public/css/',
	},
};

gulp.task('javascript', () => {
	const files = glob.sync(paths.javascript.src);

	return merge(files.map(f => {
		return browserify({ entries: f, debug: true })
				.transform('babelify', { presets: ['es2015', 'stage-2'] })
					.bundle()
					.pipe(source(path.basename(f)))
					.pipe(buffer())
					.pipe(sourcemaps.init())
					.pipe(uglify())
					.pipe(sourcemaps.write(paths.javascript.maps))
					.pipe(gulp.dest(paths.javascript.dest));
	}));
});

gulp.task('sass', () => {
	gulp.src(paths.sass.src[0])
		.pipe(sass().on('error', sass.logError))
		.pipe(gulp.dest(paths.sass.dest));
});

gulp.task('watch', () => {
	gulp.watch(paths.sass.src, ['sass']);
	gulp.watch(paths.javascript.src, ['javascript']);
});

gulp.task('default', ['sass', 'javascript']);
