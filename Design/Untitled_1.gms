
set         j/1*5/;
parameters  b(j)/1 5,2 10,3 6,4 9,5 8/
            a(j)/1 6,2 8,3 7,4 10,5 10/;
scalars N/6/
        L/6/;
variables z;
binary variables x(j);
equations
          OF
          co1
          co2
          co3;

OF        ..z=e=sum(j,b(j)*x(j))+sum(j,a(j)*x(j));
co1       ..sum(j,a(j)*x(j))=g=N;
co2       ..sum(j,b(j)*x(j))=g=L;
co3       ..sum(j,x(j))=L=1;
model makanyabi /all/;
option MIP=Cplex;
solve makanyabi using MIP Maximizing z;
display z.1,x.1;

