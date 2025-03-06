var log = console.log;

log('🎨');

var internals = {
    config: {
        totalGroups: 50
    },
    colors: [
        [0x00ffff, 0xff0000],
        [0x00ff00, 0xff00ff],
        [0x0000ff, 0xffff00]
    ]
};

internals.w = window.innerWidth;
internals.h = window.innerHeight;

internals.random = function (min, max) {
    return min + Math.random() * (max - min);
};

// -------

internals.app = new PIXI.Application({
    width: internals.w,
    height: internals.h,
    antialias: true,
    resolution: window.devicePixelRatio,
    transparent: false,
    autoResize: true,
    backgroundColor: 0xFFFFFF
});

document.body.appendChild(internals.app.view);

// -------

function createShape(id) {
    var shape = {
        index: id,
        offset: 50,
        colorsIndex: 0,
        container: new PIXI.Container(),
        graphicsContainer: new PIXI.Container(),
        graphicA: new PIXI.Graphics(),
        graphicB: new PIXI.Graphics(),
        draw: function (colorsIndex) {
            if (colorsIndex === undefined) {
                this.colorsIndex = ++this.colorsIndex % internals.colors.length;
            }

            var colors = internals.colors[this.colorsIndex];

            if (this.index % 2) {
                this.graphicA.clear();
                this.graphicA.beginFill(colors[0]);
                this.graphicA.drawRect(internals.random(0, this.offset), internals.random(0, this.offset), 60, 60);
                this.graphicA.endFill();

                this.graphicA.beginFill(colors[1]);
                this.graphicA.drawRect(internals.random(0, this.offset), internals.random(0, this.offset), 60, 60);
                this.graphicA.endFill();
            }
            else {
                this.graphicB.clear();
                this.graphicB.beginFill(colors[0]);
                this.graphicB.drawCircle(internals.random(0, this.offset), internals.random(0, this.offset), 30);
                this.graphicB.endFill();

                this.graphicB.beginFill(colors[1]);
                this.graphicB.drawCircle(internals.random(0, this.offset), internals.random(0, this.offset), 30);
                this.graphicB.endFill();
            }

            return this;
        },
        getWidth: function () {
            return this.container.children[0].width;
        },
        getHeight: function () {
            return this.container.children[0].height;
        },
        reset: function () {
            this.container.scale.set(0);
            this.container.position.set(internals.w / 2, internals.h / 2);
            this.container.rotation = 0;
            return this;
        },
        animate: function () {
            var positionX, positionY;

            var rotation = internals.random(-360, 360);
            var scale = internals.random(0.5, 1.25);
            var delay = this.index * 0.1;

            if (Math.random() > 0.5) {
                positionX = internals.random(0 - this.getWidth(), internals.w + this.getWidth());
                positionY = Math.random() > 0.5 ? internals.h + this.getHeight() : -this.getHeight();
            }
            else {
                positionX = Math.random() > 0.5 ? internals.w + this.getWidth() : -this.getWidth();
                positionY = internals.random(0 - this.getHeight(), internals.h + this.getHeight());
            }

            TweenMax.to(this.container, internals.random(2, 6), {
                pixi: {
                    positionX: positionX,
                    positionY: positionY,
                    rotation: rotation,
                    scale: scale
                },
                delay: delay,
                onComplete: function () {
                    this.reset().animate();
                }.bind(this)
            });

            return this;
        }
    };

    shape.draw();
    shape.graphicsContainer.addChild(shape.graphicA);
    shape.graphicsContainer.addChild(shape.graphicB);
    shape.container.addChild(shape.graphicsContainer);
    shape.container.pivot.x = shape.getWidth() / 2;
    shape.container.pivot.y = shape.getHeight() / 2;

    shape.reset().animate();

    return shape;
}

// -------

TweenLite.defaultEase = Power0.easeNone;

internals.shapes = [];
for (var i = 0; i < internals.config.totalGroups; i++) {
    var s = createShape(i);
    internals.shapes.push(s);
    internals.app.stage.addChild(s.container);
}

// -------

function changeColors() {
    var len = internals.shapes.length;

    for (var i = 0; i < len; i++) {
        internals.shapes[i].draw();
    }
}

function resize() {
    setTimeout(function () {
        internals.w = window.innerWidth;
        internals.h = window.innerHeight;
        internals.app.renderer.resize(internals.w, internals.h);
    }, 200);
}

function render() {
    internals.app.renderer.render(internals.app.stage);
}
