class A:
  a=5
  def __init__(self, bl):
    if bl: self.b=4

a = int(input("enter 0 or 1:"))
if a:
  a = A(a)
  a.c = 3
else:
  A.d = 4
  a = A(a)
if hasattr(a, 'c'):
  print a.c
  print a.b
if hasattr(a, 'd'): print a.d