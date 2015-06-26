using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace KindOfMagic
{
  class Instructions : List<Instruction>
  {
    public void Add(OpCode opCode)
    {
      Add(Instruction.Create(opCode));
    }

    public void InsertInto(Collection<Instruction> target, int position = 0)
    {
      foreach (var i in this)
      {
        target.Insert(position, i);
        position++;
      }
    }

    public void MergeInto(Collection<Instruction> target, int position = 0)
    {
      var first = true;

      foreach (var i in this)
      {
        if (first)
        {
          first = false;
          var op = target[position];
          op.Operand = i.Operand;
          op.OpCode = i.OpCode;
        }
        else
        {
          var op = Instruction.Create(OpCodes.Nop);
          op.Operand = i.Operand;
          op.OpCode = i.OpCode;
          target.Insert(position, op);
        }
        position++;
      }
    }

    public void Call(MethodReference method)
    {
      Add(Instruction.Create(OpCodes.Call, method));
    }

    internal void Callvirt(MethodReference method)
    {
      Add(Instruction.Create(OpCodes.Callvirt, method));
    }

    public void Ldarg_0()
    {
      Add(Instruction.Create(OpCodes.Ldarg_0));
    }

    public void Ldarg_1()
    {
      Add(Instruction.Create(OpCodes.Ldarg_1));
    }

    public void Ldfld(FieldReference field)
    {
      Add(Instruction.Create(OpCodes.Ldfld, field));
    }

    public void Stloc_S(VariableDefinition var)
    {
      Add(Instruction.Create(OpCodes.Stloc_S, var));
    }

    public Instruction Ldloc(VariableDefinition var)
    {
      var result = Instruction.Create(OpCodes.Ldloc, var);
      Add(result);
      return result;
    }

    public void Bne_Un_S(Instruction target)
    {
      Add(Instruction.Create(OpCodes.Bne_Un_S, target));
    }

    public void Bne_Un(Instruction target)
    {
      Add(Instruction.Create(OpCodes.Bne_Un, target));
    }

    public void Ceq()
    {
      Add(Instruction.Create(OpCodes.Ceq));
    }

    public void Brtrue_S(Instruction target)
    {
      Add(Instruction.Create(OpCodes.Brtrue_S, target));
    }

    public void Brfalse_S(Instruction target)
    {
      Add(Instruction.Create(OpCodes.Brfalse_S, target));
    }

    public void Ldstr(string str)
    {
      Add(Instruction.Create(OpCodes.Ldstr, str));
    }

    public void Beq_S(Instruction target)
    {
      Add(Instruction.Create(OpCodes.Beq_S, target));
    }

    public void Beq(Instruction target)
    {
      Add(Instruction.Create(OpCodes.Beq, target));
    }

    public void CastClass(TypeReference type)
    {
      Add(Instruction.Create(OpCodes.Castclass, type));
    }

    public void Ldflda(FieldReference field)
    {
      Add(Instruction.Create(OpCodes.Ldflda, field));
    }

    public void Ret()
    {
      Add(Instruction.Create(OpCodes.Ret));
    }

    public void Ldc_i4_0()
    {
      Add(Instruction.Create(OpCodes.Ldc_I4_0));
    }

    public void Ldloca_S(VariableDefinition var)
    {
      Add(Instruction.Create(OpCodes.Ldloca_S, var));
    }

    internal void Ldarga_S(ParameterDefinition param)
    {
      Add(Instruction.Create(OpCodes.Ldarga_S, param));
    }

    internal void Nop()
    {
      Add(Instruction.Create(OpCodes.Nop));
    }

    internal void Stloc_0()
    {
      Add(Instruction.Create(OpCodes.Stloc_0));
    }
  }
}
