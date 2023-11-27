/// <binding AfterBuild="" BeforeBuild="" Clean="all" ProjectOpened="" />

const del = require("del"),
    gulp = require("gulp"),
    cleanCSS = require("gulp-clean-css"),
    concat = require("gulp-concat"),
    less = require("gulp-less"),
    rename = require("gulp-rename"),
    replace = require("gulp-replace"),
/*    responsive = require("gulp-responsive"),*/
    terser = require("gulp-terser");
const options = {
    gulpSrc: {
        allowEmpty: true
    },
    rename: {
        suffix: ".min"
    },
    responsive: {
        compressionLevel: 6,
        progressive: true,
        quality: 70,
        withMetadata: false
    }
};

gulp.task("bundle:css", function () {
    return gulp.src([
        `wwwroot/assets/css/vendors.bundle.css`,
        `wwwroot/assets/css/style.bundle.css`,
        `wwwroot/assets/plugins/datatables/**.css`,
        `wwwroot/css/custom**.css`,
        `!wwwroot/assets/css/**.bundle.min.css`,
        `!wwwroot/assets/plugins/*/**.min.css`,
        `!wwwroot/css/custom**.min.css`
    ], options.gulpSrc)
        .pipe(concat({
            path: "bundle.css"
        }))
        .pipe(replace("fonts/", "../assets/css/fonts/"))
        .pipe(gulp.dest(`wwwroot/css`));
});

gulp.task("bundle:js", function () {
    return gulp.src([
        `wwwroot/assets/js/vendors.bundle.js`,
        `wwwroot/assets/js/scripts.bundle.js`,
        `wwwroot/assets/plugins/datatables/**.js`,
        `wwwroot/js/custom**.js`,
        `!wwwroot/assets/js/**.bundle.min.js`,
        `!wwwroot/assets/plugins/*/**.min.js`,
        `!wwwroot/js/custom**.min.js`
    ], options.gulpSrc)
        .pipe(concat({
            path: "bundle.js"
        }))
        .pipe(gulp.dest(`wwwroot/js`));
});

gulp.task("bundle:css:clean", function () {
    return gulp.src([
        `wwwroot/assets/css/vendors.clean.bundle.css`,
        `wwwroot/assets/css/style.clean.bundle.css`,
        `wwwroot/assets/plugins/datatables/**.css`,
        `wwwroot/css/custom**.css`,
        `!wwwroot/assets/css/**.clean.bundle.min.css`,
        `!wwwroot/assets/plugins/*/**.min.css`,
        `!wwwroot/css/custom**.min.css`
    ], options.gulpSrc)
        .pipe(concat({
            path: "bundle.clean.css"
        }))
        .pipe(replace("fonts/", "../assets/css/fonts/"))
        .pipe(gulp.dest(`wwwroot/css`));
});

gulp.task("bundle:js:clean", function () {
    return gulp.src([
        `wwwroot/assets/js/vendors.clean.bundle.js`,
        `wwwroot/assets/js/scripts.clean.bundle.js`,
        `wwwroot/assets/plugins/datatables/**.js`,
        `wwwroot/js/custom**.js`,
        `!wwwroot/assets/js/**.clean.bundle.min.js`,
        `!wwwroot/assets/plugins/*/**.min.js`,
        `!wwwroot/js/custom**.min.js`
    ], options.gulpSrc)
        .pipe(concat({
            path: "bundle.clean.js"
        }))
        .pipe(gulp.dest(`wwwroot/js`));
});

gulp.task("bundle:css:slim", function () {
    return gulp.src([
        `wwwroot/assets/css/vendors.slim.bundle.css`,
        `wwwroot/assets/css/style.slim.bundle.css`,
        `wwwroot/assets/plugins/datatables/**.css`,
        `wwwroot/css/custom**.css`,
        `!wwwroot/assets/css/**.slim.bundle.min.css`,
        `!wwwroot/assets/plugins/*/**.min.css`,
        `!wwwroot/css/custom**.min.css`
    ], options.gulpSrc)
        .pipe(concat({
            path: "bundle.slim.css"
        }))
        .pipe(replace("fonts/", "../assets/css/fonts/"))
        .pipe(gulp.dest(`wwwroot/css`));
});

gulp.task("bundle:js:slim", function () {
    return gulp.src([
        `wwwroot/assets/js/vendors.slim.bundle.js`,
        `wwwroot/assets/js/scripts.slim.bundle.js`,
        `wwwroot/assets/plugins/datatables/**.js`,
        `wwwroot/js/custom**.js`,
        `!wwwroot/assets/js/**.slim.bundle.min.js`,
        `!wwwroot/assets/plugins/*/**.min.js`,
        `!wwwroot/js/custom**.min.js`
    ], options.gulpSrc)
        .pipe(concat({
            path: "bundle.slim.js"
        }))
        .pipe(gulp.dest(`wwwroot/js`));
});

gulp.task("clean:css", async function () {
    return await del([
        `wwwroot/css/themes/**/*.css`,
        `wwwroot/css/bundle**.css`
    ]);
});

gulp.task("clean:js", async function () {
    return await del([
        `wwwroot/js/themes/**/*.js`,
        `wwwroot/js/bundle**.js`
    ]);
});

gulp.task("clean:css:min", async function () {
    return await del([
        `wwwroot/css/areas/**/*.min.css`,
        `wwwroot/css/themes/**/*.min.css`,
        `wwwroot/css/views/**/*.min.css`,
        `wwwroot/css/*.min.css`
    ]);
});

gulp.task("clean:js:min", async function () {
    return await del([
        `wwwroot/js/areas/**/*.min.js`,
        `wwwroot/js/themes/**/*.min.js`,
        `wwwroot/js/views/**/*.min.js`,
        `wwwroot/js/*.min.js`
    ]);
});

gulp.task("compile:less", function () {
    return gulp.src([
        `wwwroot/css/themes/*/**.less`,
        `!wwwroot/css/themes/*/**/colors.less`
    ], options.gulpSrc)
        .pipe(less())
        .pipe(gulp.dest(`wwwroot/css/themes`));
});

gulp.task("min:css", function () {
    return gulp.src([
        `wwwroot/css/**/*.css`,
        `!wwwroot/css/**/*.min.css`,
        `!wwwroot/css/lib/**/*.css`,
        `!wwwroot/css/modules/**/*.css`
    ], options.gulpSrc)
        .pipe(cleanCSS())
        .pipe(rename(options.rename))
        .pipe(gulp.dest(`wwwroot/css`));
});

gulp.task("min:js", async function () {
    return gulp.src([
        `wwwroot/js/**/*.js`,
        `!wwwroot/js/**/*.min.js`,
        `!wwwroot/js/lib/**/*.js`,
        `!wwwroot/js/modules/**/*.js`
    ], options.gulpSrc)
        .pipe(rename(options.rename))
        .pipe(terser())
        .pipe(gulp.dest(`wwwroot/js`));
});

//gulp.task("responsive:image", function () {
//    return gulp.src([
//        `wwwroot/images/**/*.jpg`,
//        `!wwwroot/images/demo/**/*.js`
//    ], options.gulpSrc)
//        .pipe(responsive({
//            "login.jpg": {
//                width: 1200,
//            },
//            "logo.png": {
//                rename: {
//                    suffix: "-200"
//                },
//                width: 300,
//            }
//        }, options.responsive))
//        .pipe(gulp.dest(`wwwroot/images`));
//});

gulp.task("watch:less", async function () {
    return gulp.watch([`wwwroot/css/themes/*/**.less`], "all");
});

gulp.task("bundle", gulp.parallel(["bundle:css:slim", "bundle:css:clean", "bundle:css", "bundle:js:slim", "bundle:js:clean", "bundle:js"]));
gulp.task("clean", gulp.parallel(["clean:css:min", "clean:css", "clean:js:min", "clean:js"]));
gulp.task("compile", gulp.parallel(["compile:less"]));
gulp.task("min", gulp.parallel(["min:css", "min:js"]));
gulp.task("watch", gulp.parallel(["watch:less"]));
gulp.task("all", gulp.series(["clean", "bundle", "compile", "min"]));
