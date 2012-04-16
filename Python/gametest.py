#!/usr/bin/python
# -*- coding: utf-8 -*-

from game import Board, PROB_LBORDER, PROB_PIN, PROB_RBORDER, PinAt, Point, Walker
import unittest

__all__ = ['TestMakeCell', 'TestBoard', 'TestWalkerPrimitives', 'TestWalkerTrajectories']


class _CustomTest(unittest.TestCase):

    def assertTrue(self, cond, msg=None):
        msg = msg or 'Assertion failed!'
        unittest.TestCase.assertTrue(self, cond, msg)


class TestMakeCell(_CustomTest):

    def test_make_pin(self):
        cell = PinAt(1, max=2)
        self.assertEquals(cell, PROB_PIN)

    def test_make_rborder_pin(self):
        cell = PinAt(2, max=2)
        self.assertEquals(cell, PROB_RBORDER)

    def test_make_lborder_pin(self):
        cell = PinAt(0, max=2)
        self.assertEquals(cell, PROB_LBORDER)


class TestBoardValidation(_CustomTest):

    _BAD_HEIGHT_MSG = 'La altura debe estar entre 3 y 99'
    _BAD_WIDTH_MSG = 'El ancho debe estar entre 3 y 100'

    def test_too_short(self):
        with self.assertRaises(ValueError) as ctx:
            Board(0, 3)
        self.assertEqual(ctx.exception.message, self._BAD_HEIGHT_MSG)

    def test_too_long(self):
        with self.assertRaises(ValueError) as ctx:
            Board(100, 3)
        self.assertEqual(ctx.exception.message, self._BAD_HEIGHT_MSG)

    def test_too_narrow(self):
        with self.assertRaises(ValueError) as ctx:
            Board(3, 0)
        self.assertEqual(ctx.exception.message, self._BAD_WIDTH_MSG)

    def test_too_wide(self):
        with self.assertRaises(ValueError) as ctx:
            Board(3, 101)
        self.assertEqual(ctx.exception.message, self._BAD_WIDTH_MSG)

    def test_length_shouldbe_odd(self):
        with self.assertRaises(ValueError) as ctx:
            Board(4, 5)
        self.assertEqual(ctx.exception.message, 'La altura debe ser impar')

    def test_make_min_board(self):
        board = Board(3, 3)
        self.assertEquals(3, board.height, "board's height")
        self.assertEquals(3, board.width, "board's width")

    def test_make_max_board(self):
        board = Board(99, 100)
        self.assertEquals(99, board.height, "board's height")
        self.assertEquals(100, board.width, "board's width")


class TestBoard(_CustomTest):

    def setUp(self):
        self.b = Board(3, 3)

    def test_constructor(self):
        self.assertIsNotNone(self.b)

    def test_cells_default_to_blank(self):
        self.assertFalse(self.b._is_pin(1, 0))

    def test_set_pin_at(self):
        self.b._set_pin_at(1, 0)
        self.assertTrue(self.b._is_pin(1, 0))

    def test_SetPinAtLeftBorder(self):
        self.b._set_pin_at(1, 0);
        self.assertEquals(self.b[1, 0], PROB_LBORDER)

    def test_SetPinClear(self):
        self.b._set_pin_at(2, 1)
        self.assertEquals(self.b[2, 1], PROB_PIN);

    def test_SetPinAtRightBorder(self):
        self.b._set_pin_at(1, 2)
        self.assertEquals(self.b[1, 2], PROB_RBORDER);

    def test_default_pin_distribution(self):
        expected = 'x.x\n' \
                   '.x.\n' \
                   'x.x'
        self.assertEquals(expected, str(self.b))

    def test_remove_pin(self):
        self.b.remove_pin_at(1, 1)
        self.assertFalse(self.b._is_pin(1, 1))


class TestWalkerPrimitives(_CustomTest):

    def setUp(self):
        self.w = Walker(Board(3, 3))

    def assertNextStepFrom(self, point, should_be):
        self.assertEqual(should_be, self.w._next_step(point))

    def test_get_next_step_if_blank(self):
        expected = [(Point(1, 1), 1)]
        self.assertNextStepFrom(Point(0, 1), should_be=expected)

    def test_get_next_step_if_lborder(self):
        expected = [(Point(0, 1), 1)]
        self.assertNextStepFrom(Point(0, 0), should_be=expected)

    def test_get_next_step_if_rborder(self):
        expected = [(Point(2, 1), 1)]
        self.assertNextStepFrom(Point(2, 2), should_be=expected)

    def test_get_next_step_if_pin(self):
        expected = [(Point(1, 0), 0.5), (Point(1, 2), 0.5)]
        self.assertNextStepFrom(Point(1, 1), should_be=expected)


class TestWalkerRoutes(_CustomTest):

    def _test_straight_trajectory(self):
        w = Walker(Board(3, 3).remove_pin_at(1, 1))

        EXPECTED_PROB = 1
        probability = w.starting_from(0, 1).can_arribe_at(2, 1)

        self.assertEquals(EXPECTED_PROB, probability)

    def test_single_hop_trajectory(self):
        w = Walker(Board(3, 3))

        EXPECTED_PROB = 0.5
        probability = w.starting_from(0, 1).can_arribe_at(2, 1)

        self.assertEquals(EXPECTED_PROB, probability)

    def test_two_hop_trajectory(self):
        w = Walker(Board(5, 5))

        EXPECTED_PROB = 0.25
        probability = w.starting_from(0, 3).can_arribe_at(2, 1)

        self.assertEquals(EXPECTED_PROB, probability)


if __name__ == '__main__':
    unittest.main()
