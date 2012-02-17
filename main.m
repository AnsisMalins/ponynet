img = imread('twilight.png');

% Make data fit for computation.
X = single([img(:, :, 1)(:) img(:, :, 2)(:) img(:, :, 3)(:)]) / 255;
y = single(255 - img(:, :, 4)(:)) / 255;
imshow(reshape(X .* [y y y], size(img, 1), size(img, 2), 3));