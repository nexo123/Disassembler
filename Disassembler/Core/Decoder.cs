using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;


namespace Disassembler.Core
{
    class Decoder
    {
        public byte[] machine_code { get; set; }
        protected Dictionary<string, Instruction> instruction_set;

        private Dictionary<string, string> registers = new Dictionary<string, string>()
        {
            { "0000", "AL" },
            { "0010", "CL" },
            { "0100", "DL" },
            { "0110", "BL" },
            { "1000", "AH" },
            { "1010", "CH" },
            { "1100", "DH" },
            { "1110", "BH" },
            { "0001", "AX" },
            { "0011", "CX" },
            { "0101", "DX" },
            { "0111", "BX" },
            { "1001", "SP" },
            { "1011", "BP" },
            { "1101", "SI" },
            { "1111", "DI" }
        };

        private Dictionary<string, string> segment_registers = new Dictionary<string, string>()
        {
            { "000", "ES" },
            { "001", "CS" },
            { "010", "SS" },
            { "011", "DS" }
        };

        private Dictionary<string, string> GRP1 = new Dictionary<string, string>()
        {
            { "000", "ADD" },
            { "001", "OR" },
            { "010", "ADC" },
            { "011", "SBB" },
            { "100", "AND" },
            { "101", "SUB" },
            { "110", "XOR" },
            { "111", "CMP" }
        };

        private Dictionary<string, string> GRP2 = new Dictionary<string, string>()
        {
            { "000", "ROL" },
            { "001", "ROR" },
            { "010", "RCL" },
            { "011", "RCR" },
            { "100", "SHL" },
            { "101", "SHR" },
            { "111", "SAR" }
        };

        private Dictionary<string, string> GRP3A = new Dictionary<string, string>()
        {
            { "000", "TEST" },
            { "010", "NOT" },
            { "011", "NEG" },
            { "100", "MUL" },
            { "101", "IMUL" },
            { "110", "DIV" },
            { "111", "IDIV" }
        };

        private Dictionary<string, string> GRP3B = new Dictionary<string, string>()
        {
            { "000", "TEST" },
            { "010", "NOT" },
            { "011", "NEG" },
            { "100", "MUL" },
            { "101", "IMUL" },
            { "110", "DIV" },
            { "111", "IDIV" }
        };

        private Dictionary<string, string> GRP4 = new Dictionary<string, string>()
        {
            { "000", "INC" },
            { "001", "DEC" }
        };

        private Dictionary<string, string> GRP5 = new Dictionary<string, string>()
        {
            { "000", "INC" },
            { "001", "DEC" },
            { "010", "CALL" },
            { "011", "CALL" },
            { "100", "JMP" },
            { "101", "JMP" },
            { "110", "PUSH" }
        };

        private Dictionary<string, string> addressing_modes = new Dictionary<string, string>()
        {
            { "000", "BX+SI" },
            { "001", "BX+DI" },
            { "010", "BP+SI" },
            { "011", "BX+DI" },
            { "100", "SI" },
            { "101", "DI" },
            { "110", "BP" },
            { "111", "BX" }
        };

        //initialize instruction set at startup
        public Decoder()
        {
            InitInstructionSet();
        }

        /// <summary>
        /// Simple method to parse input JSON file with 8086 instruction set and initialize the instruction set for the decoder.
        /// </summary>
        private void InitInstructionSet()
        {
            instruction_set = new Dictionary<string, Instruction>();
            string json = File.ReadAllText("instruction_set.json");
            var tmp = JsonConvert.DeserializeObject<List<Instruction>>(json);
            foreach (Instruction instruction in tmp)
            {
                instruction_set.Add(instruction.opcode, instruction);
            }
        }

        /// <summary>
        /// Main decoding method. Tries to decode instruction at offset specified by param.
        /// </summary>
        /// <param name="ip">Specifies the offset pointing to first byte of the instruction to decode.</param>
        /// <returns>Returns Instruction structure with either the decoded instruction or error message in the "name" attribute.</returns>
        public Instruction DecodeInstructionAt(int ip)
        {
            Instruction result = new Instruction(); //Instruction structure to hold the result
            string first_byte = "";
            string second_byte = "";
            string rm = "";
            string mod = "";
            string reg = "";

            //basic tests 
            if (machine_code == null)
            {
                result.length = 0;
                result.name = "Machine code not initialized! Array == null.";
                return result;
            }
            if (ip < 0 || ip >= machine_code.Length)
            {
                result.length = 0;
                result.name = "Instruction pointer out of range! IP: " + ip.ToString("X4") + "H";
                return result;
            }

            //read first byte
            first_byte = Convert.ToString(machine_code[ip], 16).PadLeft(2, '0').ToUpper();

            //get the instruction based on opcode (first_byte), if successfull proceed with decoding
            if (instruction_set.TryGetValue(first_byte, out Instruction found))
            {
                //add lenght
                result.length += found.length;

                //test if instruction has a second byte
                if (found.length == 2)
                {
                    //check if the second byt is withing buffer
                    if ((ip + 1) < machine_code.Length)
                    {
                        //read second byte
                        second_byte = Convert.ToString(machine_code[ip + 1], 2).PadLeft(8, '0');
                    }
                    else
                    {
                        //handle out of range error
                        result.length = 0;
                        result.name = "Instruction pointer out of range! IP: " + ip.ToString("X4") + "H";
                        return result;
                    }

                    //get rm, reg and mod fields
                    mod = second_byte.Substring(0, 2);
                    reg = second_byte.Substring(2, 3);
                    rm = second_byte.Substring(5, 3);

                    //test for opcode extension
                    if (found.name.Contains("GRP"))
                    {
                        string name = DecodeOpcodeExtension(reg, found.name);
                        if (name.Length > 0)
                        {
                            result.name = name;
                        }
                        else
                        {
                            result.name = "Failed to decode opcode extension at " + ip.ToString("X4") + "H";
                            result.length = 0;
                            return result;
                        }
                    }
                    else
                    {
                        result.name = found.name;
                    }

                    //decode operands
                    KeyValuePair<int, string> decoded_operand = new KeyValuePair<int, string>();
                    //decode first operand
                    if (found.operand1.Equals("-")) //first operand doesn't exist, set to empty string
                    {
                        result.operand1 = "";
                    }
                    else if (registers.ContainsValue(found.operand1) || segment_registers.ContainsValue(found.operand1)) //second operand is found in register dictionaries
                    {
                        result.operand1 = found.operand1;
                    }
                    else //else decode operand
                    {
                        decoded_operand = DecodeOperand(found.operand1, mod, reg, rm, ip);
                        if (decoded_operand.Key < 0)
                        {
                            //handle errors
                            result.length = 0;
                            result.name = decoded_operand.Value;
                            return result;
                        }
                        else
                        {
                            result.operand1 = decoded_operand.Value;
                            result.length += decoded_operand.Key;
                        }
                    }

                    //decode second operand
                    if (found.operand2.Equals("-")) //second operand doesn't exist, set to empty string
                    {
                        result.operand2 = "";
                    }
                    else if (registers.ContainsValue(found.operand2) || segment_registers.ContainsValue(found.operand2)) //second operand is found in register dictionaries
                    {
                        result.operand2 = found.operand2;
                    }
                    else //else decode operand
                    {
                        decoded_operand = DecodeOperand(found.operand2, mod, reg, rm, ip);
                        if (decoded_operand.Key < 0)
                        {
                            //handle errors
                            result.length = 0;
                            result.name = decoded_operand.Value;
                            return result;
                        }
                        else
                        {
                            result.operand2 = ", " + decoded_operand.Value;
                            result.length += decoded_operand.Key;
                        }
                    }
                }
                else //decode operands, no MODR/M byte
                {
                    result.name = found.name;
                    KeyValuePair<int, string> decoded_operand = new KeyValuePair<int, string>();
                    //decode first operand
                    if (found.operand1.Equals("-")) //first operand doesn't exist, set to empty string
                    {
                        result.operand1 = "";
                    }
                    else if (registers.ContainsValue(found.operand1) || segment_registers.ContainsValue(found.operand1)) //first operand is found in register dictionaries
                    {
                        result.operand1 = found.operand1;
                    }
                    else //else decode operand
                    {
                        decoded_operand = DecodeOperand(found.operand1, "XX", "XX", "XX", ip); //set _rm, _mod and _reg params to XX as we don't have MODR/M byte
                        if (decoded_operand.Key < 0)
                        {
                            //handle errors
                            result.length = 0;
                            result.name = decoded_operand.Value;
                            return result;
                        }
                        else
                        {
                            result.operand1 = decoded_operand.Value;
                            result.length += decoded_operand.Key;
                        }
                    }

                    //decode second operand
                    if (found.operand2.Equals("-")) //second operand doesn't exist, set to empty string
                    {
                        result.operand2 = "";
                    }
                    else if (registers.ContainsValue(found.operand2) || segment_registers.ContainsValue(found.operand2)) //second operand is found in register dictionaries
                    {
                        result.operand2 = found.operand2;
                    }
                    else //else decode operand
                    {
                        decoded_operand = DecodeOperand(found.operand2, "XX", "XX", "XX", ip); //set _rm, _mod and _reg params to XX as we don't have MODR/M byte
                        if (decoded_operand.Key < 0)
                        {
                            //handle errors
                            result.length = 0;
                            result.name = decoded_operand.Value;
                            return result;
                        }
                        else
                        {
                            result.operand2 = ", " + decoded_operand.Value;
                            result.length += decoded_operand.Key;
                        }
                    }
                }
            }
            else
            {
                result = new Instruction
                {
                    length = 0,
                    name = "Instruction opcode (" + first_byte + "H) not found! Please consider updating the instruction set."
                };
                return result;
            }
            return result;
        }

        /// <summary>
        /// Method decodes instruction operand.
        /// </summary>
        /// <param name="_operand">Operand code.</param>
        /// <param name="_mod">2 bit MOD field. Set to "XX" if no MOD field</param>
        /// <param name="_reg">3 bit REG field. Set to "XX" if no REG field</param>
        /// <param name="_rm">3 bit R/M field. Set to "XX" if no R/M field</param>
        /// <param name="ip">Current value of the instruction pointer.</param>
        /// <returns>Key value pair of number of bytes decoded as key + decoded operand as value.</returns>
        private KeyValuePair<int, string> DecodeOperand(string _operand, string _mod, string _reg, string _rm, int ip)
        {
            KeyValuePair<int, string> operand;
            string first = "";
            string second = "";
            if (_operand.Length > 1)
            {
                first = _operand.Substring(0, 1);
                second = _operand.Substring(1, 1);
            }
            else
            {
                first = _operand;
                second = "x";
            }

            switch (first)
            {
                case "A": //Direct address. The instruction has no ModR/M byte; the address of the operand is encoded in the instruction. Applicable, e.g., to far JMP (opcode EA).
                    operand = Decode32bAddress(ip);
                    break;
                case "E": //A ModR/M byte follows the opcode and specifies the operand. The operand is either a general-purpose register or a memory address. If it is a memory address, the address is computed from a segment register and any of the following values: a base register, an index register, a displacement.
                    operand = DecodeGeneralOperand(ip, second, _mod, _rm, _reg);
                    break;
                case "G": //The reg field of the ModR/M byte selects a general register.
                    operand = DecodeRegOperand(_reg, second);
                    break;
                case "I": //Immediate data. The operand value is encoded in subsequent bytes of the instruction.
                    operand = DecodeImmediateOperand(ip, second, _mod, _rm);
                    break;
                case "J": //The instruction contains a relative offset to be added to the address of the subsequent instruction. Applicable, e.g., to short JMP (opcode EB), or LOOP
                    operand = DecodeJumpOperand(ip, second);
                    break;
                case "M": //The ModR/M byte refers to a memory location, however the contents of that memory location are irrelevant; the address itself is the operand of the instruction. Applicable, e.g., to LEA
                    operand = DecodeGeneralOperand(ip, second, _mod, _rm, _reg);
                    break;
                case "O": //The instruction has no ModR/M byte; the offset of the operand is encoded as a WORD in the instruction. Applicable, e.g., to certain MOVs (opcodes A0 through A3).
                    operand = new KeyValuePair<int, string>(2, Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') + 
                        Convert.ToString(machine_code[ip + 1], 16).ToUpper().PadLeft(2, '0') + "H");
                    break;
                case "S": //The reg field of the ModR/ M byte selects a segment register.
                    operand = DecodeSregOperand(_reg);
                    break;
                case "1": //A constant argument of 1, implicit in the opcode, and not represented elsewhere in the instruction. This argument *is* displayed in assembly code.
                    operand = new KeyValuePair<int, string>(0, "01H");
                    break;
                case "3": //A constant argument of 3, implicit in the opcode, and not represented elsewhere in the instruction. This argument *is* displayed in assembly code.
                    operand = new KeyValuePair<int, string>(0, "03H");
                    break;
                default:
                    operand = new KeyValuePair<int, string>(-1, "Decoding operand failed! Operand code: " + _operand);
                    break;
            }
            return operand;
        }

        /// <summary>
        /// Method to decode 32b segment:offset pointer.
        /// </summary>
        /// <param name="ip">Current value of instruction pointer</param>
        /// <returns>Key value pair of number of bytes decoded as key + decoded operand as value or an error message if the decoding fails.</returns>
        private KeyValuePair<int, string> Decode32bAddress(int ip)
        {
            return ip + 4 >= machine_code.Length
                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding A-type operand!")
                : new KeyValuePair<int, string>(4, "[" + Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') +
                        Convert.ToString(machine_code[ip + 1], 16).ToUpper().PadLeft(2, '0') + ":" +
                        Convert.ToString(machine_code[ip + 4], 16).ToUpper().PadLeft(2, '0') +
                        Convert.ToString(machine_code[ip + 3], 16).ToUpper().PadLeft(2, '0') + "H]");
        }

        /// <summary>
        /// Simple method to decode segment register.
        /// </summary>
        /// <param name="_reg">MODR/M byte REG field.</param>
        /// <returns>Key value pair of number of bytes decoded as key + decoded operand as value or an error message if the decoding fails.</returns>
        private KeyValuePair<int, string> DecodeSregOperand(string _reg)
        {
            return segment_registers.TryGetValue(_reg, out string sreg)
                ? new KeyValuePair<int, string>(0, sreg)
                : new KeyValuePair<int, string>(-1, "Decoding segment register failed!");
        }

        /// <summary>
        /// Simple method to decode general register.
        /// </summary>
        /// <param name="_reg">MODR/M byte REG field.</param>
        /// <param name="second">W-bit</param>
        /// <returns>Key value pair of number of bytes decoded as key + decoded operand as value or an error message if the decoding fails.</returns>
        private KeyValuePair<int, string> DecodeRegOperand(string _reg, string second)
        {
            //we need to append either 1 or 0 to the reg field based on W bit.
            string tmp = second.Equals("w") || second.Equals("v") ? _reg + "1" : _reg + "0";
            return registers.TryGetValue(tmp, out string reg)
                ? new KeyValuePair<int, string>(0, reg)
                : new KeyValuePair<int, string>(-1, "Decoding general register failed!");
        }

        /// <summary>
        /// Simple method to decode jump instruction relative offset.
        /// </summary>
        /// <param name="ip">Current value of the instruction pointer</param>
        /// <param name="second">W-bit</param>
        /// <returns>Key value pair of number of bytes decoded as key + decoded operand as value or an error message if the decoding fails.</returns>
        private KeyValuePair<int, string> DecodeJumpOperand(int ip, string second)
        {
            if (second.Equals("w") || second.Equals("v"))
            {
                return ip + 2 >= machine_code.Length
                    ? new KeyValuePair<int, string>(-1, "Index out of range when decoding J-type operand!")
                    : new KeyValuePair<int, string>(2, Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') +
                        Convert.ToString(machine_code[ip + 1], 16).ToUpper().PadLeft(2, '0') + "H");
            }
            else
            {
                return ip + 1 >= machine_code.Length
                    ? new KeyValuePair<int, string>(-1, "Index out of range when decoding J-type operand!")
                    : new KeyValuePair<int, string>(1, Convert.ToString(machine_code[ip + 1], 16).ToUpper().PadLeft(2, '0') + "H");
            }
            
        }

        /// <summary>
        /// Method decodes E-type operand, can be register or memory based on MOD field.
        /// </summary>
        /// <param name="ip">Current value of the instruction pointer.</param>
        /// <param name="second">W-bit.</param>
        /// <param name="mod">MODR/M byte MOD field.</param>
        /// <param name="rm">MODR/M byte R/M field.</param>
        /// <param name="reg">MODR/M byte REG field.</param>
        /// <returns>Key value pair of number of bytes decoded as key + decoded operand as value or an error message if the decoding fails.</returns>
        private KeyValuePair<int, string> DecodeGeneralOperand(int ip, string second, string mod, string rm, string reg)
        {
            string addrmode = "";
            switch (mod)
            {
                case "00": //No displacement unless RM field is set to 110 = then direct 16b address
                    if (rm.Equals("110"))
                    {
                        return ip + 3 >= machine_code.Length
                            ? new KeyValuePair<int, string>(-1, "Index out of range when decoding E-type operand mod 00!")
                            : new KeyValuePair<int, string>(2, "[" + Convert.ToString(machine_code[ip + 3], 16).ToUpper().PadLeft(2, '0') +
                                Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') + "H]");
                    }
                    else
                    {
                        return addressing_modes.TryGetValue(rm, out addrmode)
                            ? new KeyValuePair<int, string>(0, "[" + addrmode + "]")
                            : new KeyValuePair<int, string>(-1, "Decoding general operand failed!");
                    }
                case "01": //8b displacement
                    if (addressing_modes.TryGetValue(rm, out addrmode))
                    {
                        return ip + 2 >= machine_code.Length
                            ? new KeyValuePair<int, string>(-1, "Index out of range when decoding E-type operand mod 01!")
                            : new KeyValuePair<int, string>(1, "[" + addrmode + "+" + Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') + "H]");
                    }
                    else
                    {
                        return new KeyValuePair<int, string>(-1, "Decoding general operand failed!");
                    }
                case "10": //16b displacement
                    if (addressing_modes.TryGetValue(rm, out addrmode))
                    {
                        return ip + 3 >= machine_code.Length
                            ? new KeyValuePair<int, string>(-1, "Index out of range when decoding E-type operand mod 10!")
                            : new KeyValuePair<int, string>(2, "[" + addrmode + "+" + Convert.ToString(machine_code[ip + 3], 16).ToUpper().PadLeft(2, '0') + 
                                Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') + "H]");
                    }
                    else
                    {
                        return new KeyValuePair<int, string>(-1, "Decoding general operand failed!");
                    }
                case "11": //Register
                    return DecodeRegOperand(rm, second);
                default:
                    return new KeyValuePair<int, string>(-1, "Decoding general operand failed!");
            }
        }

        /// <summary>
        /// Method that decodes immediate operand.
        /// </summary>
        /// <param name="ip">Current value of the instruction pointer.</param>
        /// <param name="second">W-bit</param>
        /// <param name="mod">MODR/M byte MOD field. Please set to "XX" if no MOD field is present.</param>
        /// <param name="rm">MODR/M byte R/M field. Please set to "XX" if no R/M field is present.</param>
        /// <returns>Key value pair of number of bytes decoded as key + decoded operand as value or an error message if the decoding fails.</returns>
        private KeyValuePair<int, string> DecodeImmediateOperand(int ip, string second, string mod, string rm)
        {
            switch (mod)
            {
                case "XX": //No MODR/M byte
                    if (second.Equals("w") || second.Equals("v"))
                    {
                        return ip + 2 >= machine_code.Length
                            ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                            : new KeyValuePair<int, string>(2, Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') +
                                Convert.ToString(machine_code[ip + 1], 16).ToUpper().PadLeft(2, '0') + "H");
                    }
                    else
                    {
                        return ip + 1 >= machine_code.Length
                            ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                            : new KeyValuePair<int, string>(1, Convert.ToString(machine_code[ip + 1], 16).ToUpper().PadLeft(2, '0') + "H");
                    }
                case "00": //No displacement unless RM field is set to 110 = then direct 16b address before immediate
                    if (rm.Equals("110"))
                    {
                        if (second.Equals("w") || second.Equals("v"))
                        {
                            return ip + 5 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(2, Convert.ToString(machine_code[ip + 5], 16).ToUpper().PadLeft(2, '0') +
                                    Convert.ToString(machine_code[ip + 4], 16).ToUpper().PadLeft(2, '0') + "H");
                        }
                        else
                        {
                            return ip + 4 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(1, Convert.ToString(machine_code[ip + 4], 16).ToUpper().PadLeft(2, '0') + "H");
                        }
                    }
                    else
                    {
                        if (second.Equals("w") || second.Equals("v"))
                        {
                            return ip + 3 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(2, Convert.ToString(machine_code[ip + 3], 16).ToUpper().PadLeft(2, '0') +
                                    Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') + "H");
                        }
                        else
                        {
                            return ip + 2 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(1, Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') + "H");
                        }
                    }
                case "01": //8b displacement before immediate
                    if (second.Equals("w") || second.Equals("v"))
                    {
                        return ip + 4 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(2, Convert.ToString(machine_code[ip + 4], 16).ToUpper().PadLeft(2, '0') +
                                    Convert.ToString(machine_code[ip + 3], 16).ToUpper().PadLeft(2, '0') + "H");
                    }
                    else
                    {
                        return ip + 3 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(1, Convert.ToString(machine_code[ip + 3], 16).ToUpper().PadLeft(2, '0') + "H");
                    }
                case "10": //16b displacement before immediate
                    if (second.Equals("w") || second.Equals("v"))
                    {
                        return ip + 5 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(2, Convert.ToString(machine_code[ip + 5], 16).ToUpper().PadLeft(2, '0') +
                                    Convert.ToString(machine_code[ip + 4], 16).ToUpper().PadLeft(2, '0') + "H");
                    }
                    else
                    {
                        return ip + 4 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!") 
                                : new KeyValuePair<int, string>(1, Convert.ToString(machine_code[ip + 4], 16).ToUpper().PadLeft(2, '0') + "H");
                    }
                case "11": //Immediate to register
                    if (second.Equals("w") || second.Equals("v"))
                    {
                        return ip + 3 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(2, Convert.ToString(machine_code[ip + 3], 16).ToUpper().PadLeft(2, '0') +
                                    Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') + "H");
                    }
                    else
                    {
                        return ip + 2 >= machine_code.Length
                                ? new KeyValuePair<int, string>(-1, "Index out of range when decoding I-type operand!")
                                : new KeyValuePair<int, string>(1, Convert.ToString(machine_code[ip + 2], 16).ToUpper().PadLeft(2, '0') + "H");
                    }
                default:
                    return new KeyValuePair<int, string>(-1, "Decoding immediate operand failed!");
            }
        }

        /// <summary>
        /// Simple method to decode opcode extension and transform it to actual instruction.
        /// </summary>
        /// <param name="_reg">String representing the 3 bit REG field in MODR/M byte</param>
        /// <param name="group">Opcode extension group</param>
        /// <returns>String with the instruction name.</returns>
        private string DecodeOpcodeExtension(string _reg, string group)
        {
            if (group.Equals("GRP1"))
            {
                return GRP1.TryGetValue(_reg, out string name) ? name : "";
            }
            else if (group.Equals("GRP2"))
            {
                return GRP2.TryGetValue(_reg, out string name) ? name : "";
            }
            else if (group.Equals("GRP3a"))
            {
                return GRP3A.TryGetValue(_reg, out string name) ? name : "";
            }
            else if (group.Equals("GRP3b"))
            {
                return GRP3B.TryGetValue(_reg, out string name) ? name : "";
            }
            else if (group.Equals("GRP4"))
            {
                return GRP4.TryGetValue(_reg, out string name) ? name : "";
            }
            else if (group.Equals("GRP5"))
            {
                return GRP5.TryGetValue(_reg, out string name) ? name : "";
            }
            else
            {
                return "";
            }
        }
    }


    /// <summary>
    /// Structure defined as class, holds instruction information
    /// </summary>
    public class Instruction
    {
        public string opcode { get; set; }
        public string name { get; set; }
        public int length { get; set; }
        public int address { get; set; }
        public string operand1 { get; set; }
        public string operand2 { get; set; }
    }
}
