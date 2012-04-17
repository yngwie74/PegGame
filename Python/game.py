#!/usr/bin/python
# -*- coding: utf-8 -*-

(MIN_WIDTH, MAX_WIDTH) = (3, 100)
(MIN_HEIGHT, MAX_HEIGHT) = (3, 99)

PROB_BLANK = (0, 1, 0)
PROB_PIN = (.5, 0, .5)
PROB_LBORDER = (0, 0, 1)
PROB_RBORDER = (1, 0, 0)


def PinAt(pos, max):
    if pos == 0:
        return PROB_LBORDER
    elif pos >= max:
        return PROB_RBORDER
    return PROB_PIN


def _checkWidth(c):
    if not MIN_WIDTH <= c <= MAX_WIDTH:
        raise ValueError('El ancho debe estar entre %d y %d' % (MIN_WIDTH, MAX_WIDTH))


def _checkHeight(r):
    if not MIN_HEIGHT <= r <= MAX_HEIGHT:
        raise ValueError('La altura debe estar entre %d y %d' % (MIN_HEIGHT, MAX_HEIGHT))
    if r % 2 == 0:
        raise ValueError('La altura debe ser impar')


class Board(object):

    def __init__(self, height, width):
        _checkWidth(width)
        _checkHeight(height)

        self.width = width
        self.height = height
        self._initializePins()

    def _initializePins(self):
        self.pins = {}
        for r in xrange(self.height):
            start = r % 2
            for c in xrange(start, self.width, 2):
                self._set_pin_at(r, c)
        return self

    def __getitem__(self, *args):
        p = args[0] if len(args) != 2 else Coord(*args)
        return self.pins.get(p, PROB_BLANK)

    def _is_pin(self, r, c):
        return self[r, c] is not PROB_BLANK

    def _set_pin_at(self, r, c):
        self.pins[Coord(r, c)] = PinAt(c, self.width - 1)
        return self

    def remove_pin_at(self, r, c):
        if self._is_pin(r, c):
            del self.pins[r, c]
        return self

    def in_board(self, p):
        return 0 <= p.r < self.height and 0 <= p.c < self.width

    def __str__(self):
        buffer = []
        for r in xrange(self.height):
            row = []
            for c in xrange(self.width):
                row.append('%s' % (self._is_pin(r, c) and 'x' or '.'))
            buffer.append(''.join(row))
        return '\n'.join(buffer)


class Coord(tuple):

    def __new__(cls, r, c):
        return tuple.__new__(cls, [r, c])

    r = property(lambda self: self[0])
    c = property(lambda self: self[1])


def toPoint(p):
    return p if isinstance(p, Coord) else Coord(*p)


class Result(tuple):

    def __new__(cls, col, prob):
        return tuple.__new__(cls, [col, prob])

    col = property(lambda self: self[0])
    prob = property(lambda self: self[1])

    def __gt__(self, other):
        return self.prob > other.prob


class Walker(object):

    def __init__(self, board):
        self.board = board

    def _add_step(self, point, prob, to):
        if self.board.in_board(point):
            to.append((point, prob))

    def _next_step(self, p):
        prob = self.board[p]
        result = []
        if prob[0]:
            self._add_step(Coord(p.r, p.c - 1), prob[0], to=result)
        if prob[1]:
            self._add_step(Coord(p.r + 1, p.c), prob[1], to=result)
        if prob[2]:
            self._add_step(Coord(p.r, p.c + 1), prob[2], to=result)
        return result

    def _find_highest_prob_route(self, goal, initial_prob, routes):
        if routes:
            return max(self._can_arribe_from(from_, goal, initial_prob * prob) 
                       for (from_, prob) in routes)
        return 0

    def _can_arribe_from(self, from_, to, initial_prob=1):
        if from_ == to:
            return initial_prob

        if not self.board.in_board(from_):
            return 0

        paths = self._next_step(from_)
        return self._find_highest_prob_route(to, initial_prob, paths)

    def can_arribe_from(self, r, c, to):
        return self._can_arribe_from(Coord(r, c), to, initial_prob=1)

    @property
    def _entry_cols(self):
        return xrange(1, self.board.width, 2)

    def _get_routes_to(self, to):
        walk = lambda c: self.can_arribe_from(0, c, to)
        return (Result(c / 2, walk(c)) for c in self._entry_cols)

    def find_routes_to(self, r, c):
        return self._get_routes_to(Coord(r, c))

    def find_best_route_to(self, r, c):
        return max(self.find_routes_to(r, c))


def main():
    board = Board(5, 9) \
        .remove_pin_at(1, 3) \
        .remove_pin_at(2, 2) \
        .remove_pin_at(3, 5)
    print str(board)

    w = Walker(board)
    for col, prob in w.find_routes_to(4, 3):
        print 'Entrando por %d, la probabilidad es %5.2f%%' % (col, prob * 100)


if __name__ == '__main__':
    main()
