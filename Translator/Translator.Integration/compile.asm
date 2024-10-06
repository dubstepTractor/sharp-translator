data segment para public "data"
x  dw    1
y  dw    1
PRINT_BUF DB ' ' DUP(10)
BUFEND    DB '$'
data ends
stk segment stack
db 256 dup ("?")
stk ends
code segment para public "code"
main proc
assume cs:code,ds:data,ss:stk
mov ax,data
mov ds,ax
mov ax, 0
push ax
mov ax, 1
push ax
pop bx
pop ax
or ax, bx
push ax
pop ax
not ax
push ax
pop ax
mov x, ax
mov ax, x
push ax
mov ax, 1
push ax
pop bx
pop ax
or ax, bx
push ax
mov ax, x
push ax
mov ax, 0
push ax
pop bx
pop ax
not ax
or ax, bx
push ax
pop bx
pop ax
and ax, bx
push ax
pop ax
mov y, ax
mov ax, x
push ax
CALL PRINT
pop ax
mov ax,4c00h
int 21h
main endp
PRINT PROC NEAR
MOV CX, 10
MOV DI, BUFEND - PRINT_BUF
PRINT_LOOP:
MOV DX, 0
DIV CX
ADD DL, '0'
MOV [PRINT_BUF + DI - 1], DL
DEC DI
CMP AL, 0
JNE PRINT_LOOP
LEA DX, PRINT_BUF
ADD DX, DI
MOV AH, 09H
INT 21H
RET
PRINT ENDP
code ends
end main